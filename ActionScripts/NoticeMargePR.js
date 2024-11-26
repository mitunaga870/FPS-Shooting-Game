const core = require('@actions/core');
const axios = require('axios');

(async () => {
    try {
        console.log("NoticeMargePR.js");
        
        // NotionのAPIキーを取得
        const apiKey = process.env.NOTION_API_KEY;
        core.setSecret(apiKey);
        // NotionのデータベースIDを取得
        const databaseId = process.env.NOTION_DATABASE_ID;
        core.setSecret(databaseId);
        // ヘッドブランチ名を取得
        const branch = process.env.GITHUB_HEAD_REF;
        
        // ブランチ名が取得できない場合
        if (!branch) {
            core.setFailed("Branch name not found.");
            return;
        }
        
        console.log("Branch: " + branch);
        
        // ブランチ名が登録されたタスクを取得
        const searchResult =
            await axios.put("https://api.notion.com/v1/databases/" + databaseId + "/query",
                {
                    "filter": {
                        "property": "blanch",
                        "rich_text": {
                            "equals": branch
                        }
                    }
                },
                {
                    headers: {
                        Authorization: "Bearer " + apiKey,
                        "Content-Type": "application/json",
                        "Notion-Version": "2022-06-28"
                    }
                });
        // タスクを取得
        const task = searchResult.data.results[0];
        
        // タスクが見つからない場合
        if (!task) {
            core.setFailed("Task not found.");
            return;
        }
        
        console.log("Task: " + task);
        
        // タスクのIDを取得
        const taskId = task.id;
        
        // プロパティを完了に変更
        await axios.patch("https://api.notion.com/v1/pages/" + taskId,
            {
                "properties": {
                    "ステータス": {
                        "status": {
                            "id": "ac9097b2-7f8a-400e-b6d4-f7388c1b2265"
                        }
                    }
                }
            },
            {
                headers: {
                    Authorization: "Bearer " + apiKey,
                    "Content-Type": "application/json",
                    "Notion-Version": "2022-06-28"
                }
            });
        
        console.log("NoticeMargePR.js finished.");
    } catch (error) {
        core.setFailed(error.message);
    }
})();