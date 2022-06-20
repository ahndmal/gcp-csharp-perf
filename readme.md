C# Cloud Function
Trying C# for GCP and see metrics...

deploy to GCP:

```
gcloud functions deploy dotnet2 --trigger-http --runtime=dotnet3 --entry-point=SimpleHttpFunction.Function --allow-unauthenticated --memory=256MB
```
