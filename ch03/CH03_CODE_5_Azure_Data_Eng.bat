"C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy\azcopy" /Source:C:\csvLogs\aa /Dest:https://abc.blob.core.windows.net/project-abc/v1/v1.1 /DestKey:==StorageKey== /Pattern:"ch*.csv" /XO /Y /SetContentType:text/csv