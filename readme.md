# Grpc Streamer PoC

Sample project with bi-directional communication based on [grpc](https://grpc.io/) library

## Content

1) **GrpcStreamer** project with [Protocol Buffer](https://developers.google.com/protocol-buffers/) file
2) **GrpcStreamer.Server** console app project. The server does the following: 
    - streams items to client;
    - receives status updates (as a stream) and persists updated items to the database.
3) **GrpcStreamer.Client** console app project. The client does the following: 
    - initiates bi-directional communication by opening the connection;
    - requests items in batches (the batch of items is streamed not sent at once, **top** and **skip** parameters are sent to server in headers);
    - receives a stream of items and streams status updates back to the server. 
    Retry behavior has been added so in case server is down the client will retry several times from last processed position.

## How to build and run

1) Build the solution using Visual Studio 2017
2) Create database and populate with test data using [script](https://github.com/a-yandulski/GrpcStreamer/blob/master/data/Seed.sql)
3) Start **GrpcStreamer.Server**. To stop press Ctrl+C.
4) Start **GrpcStreamer.Client**. Follow instructions.
