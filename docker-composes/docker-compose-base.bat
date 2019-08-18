set COMPOSE_PROJECT_NAME=MasterPerform
set COMPOSE_CONVERT_WINDOWS_PATHS=1
set ELASTICSEARCH_DOCKER_VERSION=7.3.0

docker-compose ^
-f network.yml ^
-f elasticsearch.yml ^
%*
