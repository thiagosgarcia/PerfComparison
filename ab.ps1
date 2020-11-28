docker-compose -f .\docker-compose-ab.yml up -d
wsl -e ab -t 60 -c 100 http://[::1]:10021/Test/Async ;
wsl -e ab -t 60 -c 100 http://[::1]:10031/Test/Async ;
wsl -e ab -t 60 -c 100 http://[::1]:10050/Test/Async ;
docker-compose -f .\docker-compose-ab.yml down