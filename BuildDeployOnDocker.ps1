//deleting BIN and OBJ folders as they have been linked to potential problems when building a docker image
del .\TradingBot\bin\* -Recurse -Force
del .\TradingBot\obj\* -Recurse -Force

//building the docker image and deploying it on the local machine
docker build -t tradingbotapp .
docker run -it --rm -p 5000:80 --name tradingbotapp_running tradingbotapp