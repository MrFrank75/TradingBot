#deleting BIN and OBJ folders as they have been linked to potential problems when building a docker image-->
del .\TradingBot\bin\* -Recurse -Force
del .\TradingBot\obj\* -Recurse -Force

#building the docker image and deploying it on the local machine
docker build -t tradingbotapp .
docker run -it --rm -p 5000:80 --name tradingbotapp_running tradingbotapp

#accessing the AWS ECR service and pushing the image there
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 286337860672.dkr.ecr.us-east-1.amazonaws.com
docker tag tradingbotapp:latest 286337860672.dkr.ecr.us-east-1.amazonaws.com/tradingbotapp:latest
docker push 286337860672.dkr.ecr.us-east-1.amazonaws.com/tradingbotapp:latest