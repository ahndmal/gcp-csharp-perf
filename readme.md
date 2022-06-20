C# Cloud Function

deploy to GCP:

```
andrii@andrii-ThinkBook-15-G2-ARE:~/prog/csharp/gcp-csharp-perf$ gcloud functions deploy dotnet2 --trigger-http --runtime=dotnet3 --entry-point=SimpleHttpFunction.Function --allow-unauthenticated --memory=256MB
```
