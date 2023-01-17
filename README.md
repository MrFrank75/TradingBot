# TradingBot
A trading bot that receives signals from TradingView and opens trades in your favourite broker system

##Build and start the TradingBot app locally, using a docker image
- clone the github repo locally
- make sure that you have docker installed and execute
	- docker build -t tradingbotapp . - to build the image containing the tradingbot
	- docker run -it --rm -p 5000:80 --name tradingbotapp_running tradingbotapp to run the image locally

- check that the web service has started accessing its swagger page https://localhost:5000/swagger/index.html
- You can also access the environment info page: http://localhost:5000/environment
- Push 

## Push the image to your favourite Cloud Provider
In case you, like me, are using AWS, you can find how to do it here: https://docs.aws.amazon.com/AmazonECR/latest/userguide/getting-started-cli.html
The basic steps (for AWS) are

- Create a repository entry for the docker image in AWS ECR
- Push the image you built before to the ECR registry

## Run the image using AWS Fargate
Once again this applies if you, like me, are using AWS.
I used this guide as a reference for the basic steps: https://docs.aws.amazon.com/AmazonECS/latest/userguide/create-container-image.html



