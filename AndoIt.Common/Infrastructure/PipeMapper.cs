using AndoIt.Common.Interface;
using System;

namespace AndoIt.Common.Infrastructure
{
    public class PipeMapper <T1, T2>
    {
        public IMapper<T1, T2> Mapper { get; private set; }
        public IDispenser<T1> Dispenser { get; private set; }
        public IProcesser<T2> Processer { get; private set; }

        public PipeMapper(IMapper<T1, T2> mapper, IDispenser<T1> dispenser, IProcesser<T2> processer)
        {
            this.Mapper = mapper ?? throw new ArgumentNullException("mapper");
            this.Dispenser = dispenser ?? throw new ArgumentNullException("dispenser");
            this.Processer = processer ?? throw new ArgumentNullException("listener");

            this.Dispenser.Dispense += this.Dispensed;
        }

        private void Dispensed(object sender, T1 toBeDispensed)
        {
            //  Do mapping
            T2 mappedObject = this.Mapper.Map(toBeDispensed);

            //  Send to prcess
            this.Processer.Process(mappedObject);
        }
    }
}
