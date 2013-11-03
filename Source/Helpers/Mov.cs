using System;
using System.Diagnostics;

namespace RandomStringGenerator.Helpers {
    internal unsafe struct Mov {
        private readonly char* _start;
        private readonly char* _end;
        private char* _current;
        [DebuggerNonUserCode]
        internal char* Current {
            get { return this._current; }
            set {
                if ( value > this.End || value < this.Start )
                    throw new IndexOutOfRangeException();
                this._current = value;
            }
        }
        [DebuggerNonUserCode]
        public char* End {
            get { return this._end; }
        }
        [DebuggerNonUserCode]
        internal char* Start {
            get { return this._start; }
        }
        [DebuggerNonUserCode]
        internal bool HasNext {
            get {
                return this._end > this._current;
            }
        }
        [DebuggerNonUserCode]
        internal char GetChar( bool increment = true ) {
            return *( increment&&this.HasNext ? this.Current++ : this.Current );
        }
        [DebuggerNonUserCode]
        internal Mov( char* start, int length ) {
            this._start = start;
            this._end = start + length;
            this._current = start;
        }
    }
}