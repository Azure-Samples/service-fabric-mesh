docker build . --network host -f web/Dockerfile -t web:dev
docker build . --network host -f rcworker/Linux.Dockerfile -t rcworker:dev
docker build . --network host -f rcworker/rotate.Linux.Dockerfile -t rcworker-rotate:dev

