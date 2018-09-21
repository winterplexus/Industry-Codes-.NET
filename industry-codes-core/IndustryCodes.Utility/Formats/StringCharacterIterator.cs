//
//  StringCharacterIterator.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using System;

namespace IndustryCodes.Utility
{
    public class StringCharacterIterator
    {
        public const char Done = '\uFFFF';

        private string text;

        private int begin;
        private int end;
        private int position;

        public StringCharacterIterator(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            SetParameters(text, 0, text.Length, 0);
        }

        public StringCharacterIterator(string text, int position)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            SetParameters(text, 0, text.Length, position);
        }

        public StringCharacterIterator(string text, int begin, int end, int position)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (begin < 0 || (begin > end) || (end > text.Length))
            {
                throw new ArgumentNullException(nameof(begin));
            }
            if (position < begin || (position > end))
            {
                throw new ArgumentNullException(nameof(position));
            }
            SetParameters(text, begin, end, position);
        }

        public void SetText(string textParameter)
        {
            if (string.IsNullOrEmpty(textParameter))
            {
                throw new ArgumentNullException(nameof(textParameter));
            }
            text = textParameter;
            begin = 0;
            end = text.Length;
            position = 0;
        }

        public char First()
        {
            position = begin;
            return Current();
        }

        public char Last()
        {
            if (end != begin)
            {
                position = end - 1;
            }
            else
            {
                position = end;
            }
            return Current();
        }

        public char SetIndex(int positionParameter)
        {
            if (position < begin || (position > end))
            {
                throw new ArgumentNullException(nameof(positionParameter));
            }
            position = positionParameter;
            return Current();
        }

        public char Current()
        {
            if (position >= begin && (position < end))
            {
                return text[position];
            }
            return Done;
        }

        public char Next()
        {
            if (position < end - 1)
            {
                position++;
                return text[position];
            }
            position = end;
            return Done;
        }

        public char Previous()
        {
            if (position > begin)
            {
                position--;
                return text[position];
            }
            return Done;
        }

        public int BeginIndex => begin;

        public int EndIndex => end;

        public int Index => position;

        private void SetParameters(string textParameter, int beginParameter, int endParameter, int positionParameter)
        {
            text = textParameter;
            begin = beginParameter;
            end = endParameter;
            position = positionParameter;
        }
    }
}