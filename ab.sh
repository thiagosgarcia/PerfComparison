docker-compose -f ./docker-compose-ab.yml up -d;
ab -t 60 -c 100 http://[::1]:10021/Test/Async ;
ab -t 60 -c 100 http://[::1]:10031/Test/Async ;
ab -t 60 -c 100 http://[::1]:10050/Test/Async ;
docker-compose -f ./docker-compose-ab.yml down;