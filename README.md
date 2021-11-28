# IIASA.FotoQuestApi
Service API for submitting and retrieving Images


- The solution is delivered with a **docker-compose.yml** file, that contains all necessary components. 
  - **API** in **.net Core 3.1.**
  - **MySQL** DataBase.
- The API is ready to **dockerized** and to be used as a container image.
  - In command prompt execute >**docker-compose -f docker-compose.yml up**.
- The API written in **.NET Core**. API is documented using the **Swagger** endpoint **(http://localhost:5000/swagger/index.html)**.
- The API have endpoints for submitting photos and their metadata.
  - /Image/UploadFile - This will return **fileId**
- The data stored in a **MySQL** database.
- The photos persisted in the file system.
- The API provides endpoints to retrieve the photos for different applications as resized versions 
  - ​/Image​/Thumbnail​/**fileId** -> 128px, 
  - ​/Image​/Small​/**fileId** -> 512px, 
  - /Image​/Large/**fileId** -> 2048px
  - An endpoint with customizable size 
    - ​/Image​/**customSize**​/**fileId**
 
- Photos that have been uploaded to the API should automatically be enhanced 
  - Brightness
  - Contrast
  - Sharpen
- The API also exposes **Health endpoint** : /health