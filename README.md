# RabbitMQ and Redis Background Job Management

This project is a way to handle long-running requests as background jobs using RabbitMQ as message queue and Redis as saga state storage. It allows you to offload expensive tasks from your application's main thread and keep track of their progress asynchronously.

## Features

- **Job queue management:** RabbitMQ is used as the message broker to manage the job queue, allowing for reliable and scalable message delivery and processing.
- **Saga state storage:** Redis is used to store the state of the job as a saga, enabling you to easily manage and monitor job progress, as well as recover from failures.
- **Configurable worker pool:** You can configure the number of workers in the pool to match your application's performance needs.
- **Easily extensible:** You can add custom message handlers to handle different types of jobs and customize the worker pool behavior to suit your needs.

## Getting Started

To use this project, you will need to have Docker and Docker Compose installed on your machine. You can then clone the repository and run the following command to start the containers:

```
docker-compose up
```

This will start the RabbitMQ and Redis containers and expose their ports on your local machine. You can then use the provided client code to interact with the message broker and job queue.

When the job is complete, its saga state will be updated to reflect its status.

## Contributing

If you find a bug or have a feature request, please open an issue or submit a pull request. We welcome contributions from the community!
