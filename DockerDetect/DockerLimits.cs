namespace DockerDetect;

public static class DockerLimits {
    const string BasePath = "/sys/fs/cgroup";

    static class Cgroup1 {
        public const string Memory = $"{BasePath}/memory/memory.limit_in_bytes";
        public const string CpuQuota = $"{BasePath}/cpu/cpu.cfs_quota_us";
        public const string CpuPeriod = $"{BasePath}/cpu/cpu.cfs_period_us";
    }

    static class Cgroup2 {
        public const string Memory = $"{BasePath}/memory.max";
        public const string CpuQuota = $"{BasePath}/cpu.max";
    }

    /// <summary>
    /// Reads container CPU and memory limits. Returns 0 if the limit is not set.
    /// CPU limit returned in fractional CPU allocated (0.5, 1, 1.5, etc.).
    /// Memory limit returned in bytes.
    /// </summary>
    /// <returns>Tuple with CPU and memory limits</returns>
    public static (decimal Cpu, long Memory) GetLimits() {
        if (File.Exists(Cgroup2.Memory)) {
            // Use cgroup2
            return (ReadCpu(Cgroup2.CpuQuota), ReadLong(Cgroup2.Memory));
        }
        
        // Use cgroup1
        var cpuQuota = ReadLong(Cgroup1.CpuQuota);
        var cpuPeriod = ReadLong(Cgroup1.CpuPeriod);
        var cpu = cpuQuota == 0 || cpuPeriod == 0 ? 0 : (decimal) cpuQuota / cpuPeriod;
        return (cpu, ReadLong(Cgroup1.Memory));
    }

    static long ReadLong(string path) {
        try {
            var text = File.ReadAllText(path);
            return text == "max" ? 0 : long.Parse(text);
        }
        catch (Exception) {
            return 0;
        }
    }
    
    static decimal ReadCpu(string path) {
        try {
            var text = File.ReadAllText(path);
            var parts = text.Split(' ');
            return parts[0] == "max" ? 0 : decimal.Parse(parts[0]) / decimal.Parse(parts[1]);
        }
        catch (Exception) {
            return 0;
        }
    }
}