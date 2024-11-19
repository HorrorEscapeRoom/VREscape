using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.Safe.SoundUtility;

namespace Assets.Scripts.Safe
{
    public class SoundHandler
    {
        readonly SoundUtility soundUtility;

        public SoundHandler(SoundUtility soundUtility) => this.soundUtility = soundUtility;

        public void PlayCorrectSound() => soundUtility.PlaySound(SoundType.CorrectNumber);

        public void PlayIncorrectSound() => soundUtility.PlaySound(SoundType.IncorrectNumber);
    }
}
