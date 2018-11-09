docker build . -f web\Dockerfile -t web:dev
docker build . -f rcworker\Dockerfile -t rcworker:dev
docker build . -f rcworker\rotate.Dockerfile -t rcworker-rotate:dev