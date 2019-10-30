using System.Collections.Generic;
using System.Management;

namespace SimpleFileServer.Tools
{
    internal static class HardwareHelper
    {
        //获取移动磁盘列表
        public static List<string> GetRemovableDisk()
        {
            var removableDiskList = new List<string>();

            var mc = new ManagementClass("Win32_DiskDrive");
            var disks = mc.GetInstances();
            foreach (var moItem in disks)
            {
                var mo = (ManagementObject) moItem;
                if (mo?.Properties["MediaType"].Value == null ||
                    mo.Properties["MediaType"].Value.ToString() != "External hard disk media")
                {
                    continue;
                }

                //Console.WriteLine();
                //Console.WriteLine();
                //foreach (var prop in mo.Properties)
                //{
                //    Console.WriteLine(prop.Name + "\t" + prop.Value);
                //}

                foreach (var diskPartitionItem in mo.GetRelated("Win32_DiskPartition"))
                {
                    var diskPartition = (ManagementObject) diskPartitionItem;
                    if (diskPartition == null)
                    {
                        continue;
                    }

                    foreach (var disk in diskPartition.GetRelated("Win32_LogicalDisk"))
                    {
                        removableDiskList.Add(disk.Properties["Name"].Value.ToString());
                    }
                }
            }

            return removableDiskList;
        }

        //获取CPU列表
        public static List<string> GetCpuId()
        {
            var cpuIdList = new List<string>();

            var mc = new ManagementClass("Win32_Processor");
            var disks = mc.GetInstances();
            foreach (var moItem in disks)
            {
                var mo = moItem as ManagementObject;
                if (mo == null)
                {
                    continue;
                }

                //Console.WriteLine();
                //Console.WriteLine();
                //foreach (var prop in mo.Properties)
                //{
                //    Console.WriteLine(prop.Name + "\t" + prop.Value);
                //}
                
                cpuIdList.Add(mo.Properties["ProcessorId"].Value.ToString());
            }

            return cpuIdList;
        }
    }
}
