using System;

namespace RandomStringGenerator.Helpers {
    internal unsafe struct Mov {
        private char* _start;
        private char* _end;
        private char* _current;
        private int _read;

        internal int Read {
            get { return this._read; }
        }

        internal char* Current {
            get { return this._current; }
            set {
                if ( value > this.End || value < this.Start )
                    throw new IndexOutOfRangeException();
                this._current = value;
            }
        }

        public char* End {
            get { return this._end; }
        }

        public char* Start {
            get { return this._start; }
        }

        internal bool HasNext {
            get {
                return this._end >= this._current;
            }
        }

        internal char GetChar() {
            ++this._read;
            return *( this.Current++ );
        }

        internal Mov( char* start, int length ) {
            this._start = start;
            this._end = start + length;
            this._current = start;
            this._read = 0;
        }
    }
}