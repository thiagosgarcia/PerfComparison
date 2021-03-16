FROM ubuntu
RUN apt update &&\
		apt install apache2-utils -y
CMD	ab -t 10 -c 100 http://host.docker.internal:10021/Test/Async && \
		ab -t 10 -c 100 http://host.docker.internal:10031/Test/Async && \
		ab -t 10 -c 100 http://host.docker.internal:10050/Test/Async
