# Backend

## Endpoints

- Register Box
- Update Box
- Get Box
- Delete Box
- Get boxes (Geojson)
- Upload Image
- Get Images

## Models

BoxResponse:

- Name
- BoxId
- Geolocation
- BoxType
- CreatedAt
- IsArchived
- LatestTweet -> TweetResponse

TweetReponse :

- BoxId
- SasUri
- UploadedAt
- IsOccupied
- BirdType?
- EggCount?
- HatchedCount?
- DeadCount?
- Description
