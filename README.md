# ImageServer

Shelland ImageServer was designed to solve a problem of image thumbnail creation. It serves as a separated web application available through the REST API.

# Usage

ImageServer can be deployed using Docker container.

## Installation

*(This section is in progress)*

## HTTP API

*(This section is in progress)*

ImageServer provides the following API endpoints:

#### Create a new upload

    - POST /upload    

This endpoint accepts a *multipart/form-data* request with the following parameters:

 - File (**binary**) - file representation as it usually used in multipart requests
 - Thumbnails (**string**) - a JSON representation of the thumbnails array object:

        [{"width":320},{"width":640, "height": 480}]

#### Get information about upload

    GET /upload/{id}

Server returns a unique identifier of each upload. If you'll need to get that information again, you can call this endpoint passing an identifier to get information about processed thumbnails.

## Server response

    {
	    "id":"04ca2c4a-e84c-47b3-97f7-d03839d0d6f1",
	    "thumbnails":
	    [
		    {
			    "width":320,
			    "height":480,
			    "diskPath":"/tmp/0/4/c/04ca2c4ae84c47b397f7d03839d0d6f1_thumb_320x480.jpg",
		        "url":"http://localhost:55001/img/0/4/c/04ca2c4ae84c47b397f7d03839d0d6f1_thumb_320x480.jpg"
		     },
			 {
			    "width":640,
			    "height":480,
			    "diskPath":"/tmp/0/4/c/04ca2c4ae84c47b397f7d03839d0d6f1_thumb_640x480.jpg",	
			    "url":"http://localhost:55001/img/0/4/c/04ca2c4ae84c47b397f7d03839d0d6f1_thumb_640x480.jpg"
		    }
		]
	}


