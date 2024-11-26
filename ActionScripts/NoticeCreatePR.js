const core = require('@actions/core');
const axios = require('axios');

(async () => {
    try {
        // NotionのAPIキーを取得
        const apiKey = core.getInput('apiKey');
        // NotionのデータベースIDを取得
        const databaseId = core.getInput('databaseId');
        // ヘッドブランチ名を取得
        const branch = process.env.GITHUB_HEAD_REF;
        
        // ブランチ名が取得できない場合
        if (!branch) {
            core.setFailed("Branch name not found.");
            return;
        }
        
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
        
        // タスクのIDを取得
        const taskId = task.id;
        
        // プロパティをプルリク更新待ちに変更
        await axios.patch("https://api.notion.com/v1/pages/" + taskId,
            {
                "properties": {
                    "ステータス": {
                        "status": {
                            "id": "HZ>a"
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
    } catch (error) {
        core.setFailed(error.message);
    }
})();