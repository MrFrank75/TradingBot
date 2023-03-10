# TradingBot
A trading bot that receives signals from TradingView and opens trades in your favourite broker system

## Build and start the TradingBot app locally, using a docker image
- clone the github repo locally
- make sure that you have docker installed and execute
	- docker build -t tradingbotapp . - to build the image containing the tradingbot
	- docker run -it --rm -p 5000:80 --name tradingbotapp_running tradingbotapp to run the image locally

- check that the web service has started, by accessing its swagger page https://localhost:5000/swagger/index.html
- You can also access the environment info page: http://localhost:5000/environment

## Push the image to your favourite Cloud Provider
In case you, like me, are using AWS, you can find how to do it here: https://docs.aws.amazon.com/AmazonECR/latest/userguide/getting-started-cli.html
The basic steps (for AWS) are:

- Create a repository entry for the docker image in AWS ECR
- Push the image you built before onto the ECR registry

## Run the image using AWS Fargate
If you, like me, are using AWS, then you can publish it on the web by configuring Fargate to run your container image. 
I used this guide as a reference for the basic steps: https://docs.aws.amazon.com/AmazonECS/latest/userguide/create-container-image.html



