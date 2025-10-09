using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using _project.ScriptableObjects.Scripts;
using JetBrains.Annotations;

namespace _project.Scripts
{
    public class Sequence
    {
        private int _beatCounter;
        public List<SequenceData.SequenceAction> sequenceData = new();

        private Dictionary<int, SequenceData.SequenceAction> _sequences = new();


        private Sequence(SequenceData sequenceData)
        {
            _beatCounter = 0;
            foreach (SequenceData.SequenceAction action in sequenceData)
            {
                this.sequenceData.Add(action);
            }
        }
        
        public void Initialize()
        {
            int beatCount = 0;
            foreach (SequenceData.SequenceAction action in sequenceData)
            {
                beatCount += action.BeatDelayToPrevious;
                _sequences.Add(beatCount, action);
            }
        }

        public Sequence DoBeat()
        {
            _beatCounter++;
            return this;
        }

        public bool HasActionOnBeat([MaybeNullWhen(false)] out SequenceData.SequenceAction result)
        {
            return PeakBeat(0, out result);
        }

        public static Sequence FromSequenceData(SequenceData sequenceData)
        {
            return new Sequence(sequenceData);
        }
        
        public bool PeakBeat(int peakCount, [MaybeNullWhen(false)] out SequenceData.SequenceAction result)
        {
            return _sequences.TryGetValue(_beatCounter + peakCount, out result);
        }
    }
}