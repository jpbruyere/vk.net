using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Vulkan
{
    public unsafe class FixedUtf8String : IDisposable
    {
        GCHandle _handle;
        uint _numBytes;

        public byte* StringPtr => (byte*)_handle.AddrOfPinnedObject().ToPointer();

        public FixedUtf8String(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            byte[] text = Encoding.UTF8.GetBytes(s + "\0");
            _handle = GCHandle.Alloc(text, GCHandleType.Pinned);
            _numBytes = (uint)text.Length;
        }

        public void SetText(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            _handle.Free();
            byte[] text = Encoding.UTF8.GetBytes(s);
            _handle = GCHandle.Alloc(text, GCHandleType.Pinned);
            _numBytes = (uint)text.Length;
        }

        private string GetString()
        {
            return Encoding.UTF8.GetString(StringPtr, (int)_numBytes);
        }

        //public void Dispose()
        //{
        //    _handle.Free();
        //}

        public static implicit operator byte* (FixedUtf8String utf8String) => utf8String.StringPtr;
        public static implicit operator IntPtr (FixedUtf8String utf8String) => utf8String._handle.AddrOfPinnedObject ();
        public static implicit operator FixedUtf8String(string s) => new FixedUtf8String(s);
        public static implicit operator string(FixedUtf8String utf8String) => utf8String.GetString();

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: supprimer l'état managé (objets managés).
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.
                _handle.Free ();
                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        ~FixedUtf8String() {
          // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
          Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose () {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose (true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
             GC.SuppressFinalize(this);
        }
        #endregion
    }
}