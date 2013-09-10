all: a.out

a.out: server.cc
	g++ -L /usr/lib64 -lboost_system-mt server.cc

new:
	rm a.out *.xml
	make
	
clean:
	rm *.xml
 
