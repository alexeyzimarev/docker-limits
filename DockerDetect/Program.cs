using DockerDetect;

var limits = DockerLimits.GetLimits();
var cpuLimit = limits.Cpu == 0 ? "max" : $"{limits.Cpu} CPU";
var memoryLimit = limits.Memory == 0 ? "max" : $"{limits.Memory / 1024 / 1024} MB";

Console.WriteLine($"CPU limit: {cpuLimit}");
Console.WriteLine($"Memory limit: {memoryLimit}");