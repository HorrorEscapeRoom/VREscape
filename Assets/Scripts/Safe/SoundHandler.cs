using System;
using static Assets.Scripts.Safe.SoundUtility.SoundType;

namespace Assets.Scripts.Safe
{
    public class SoundHandler
    {
        private readonly SoundUtility soundUtility;

        public SoundHandler(SoundUtility soundUtility)
        {
            this.soundUtility = soundUtility != null ? soundUtility : throw new ArgumentNullException(nameof(soundUtility));
        }

        public void PlayCorrectSound() => soundUtility.PlaySound(CorrectNumber);
        public void PlayIncorrectSound() => soundUtility.PlaySound(IncorrectNumber);
        public void PlayMovingSound() => soundUtility.PlaySound(Moving);
    }
}