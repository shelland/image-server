docker build -t shelland/image-server -f Shelland.ImageServer\Dockerfile .
docker tag shelland/image-server:$CURRENT_TAG$ shelland/image-server:$NEW_TAG$
docker push shelland/image-server:$NEW_TAG$