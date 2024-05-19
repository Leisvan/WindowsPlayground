using GoogleBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooks.Sources
{
    public interface IBooksSource
    {
        Task<List<IBook>> GetBooksAsync(string query, int maxResults = 30);
    }
}
