# Docker limits

When executed in a Docker container, this program reads CPU and memory limits for the container, and prints them to the console.
The code makes no distinction between being unable to read the limits and the limits being set to `max` (no limit). When it returns zeros, using host machine resources is a possibility.

## How it works

The `DockerLimits` class reads the `/sys/fs/cgroup` directory to find the limits. There are two versions of `cgroup`, and the latest version is used by default. If the latest version is not found, the program falls back to the older version.

## Usage

The `DockerLimits.GetLimits` method returns a tuple with the CPU and memory limits. Memory limit is in bytes, and CPU limit is in shares (fraction allocation).

Try it locally by building the Docker image and running it:

```bash
docker build -f ./DockerDetect/Dockerfile -t detect .
docker run detect
```

The command above doesn't set any limits, so the output will be:

```
CPU limit: max
Memory limit: max
```

To set limits, use the `--cpus` and `--memory` flags:

```bash
docker run --cpus 0.5 --memory 512m detect
```

Output:

```
CPU limit: 0.5 CPU
Memory limit: 512 MB
```