using Licenta.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.DashboardManager
{
    public class DashboardManager : IDashboardManager
    {
        private readonly ApplicationDbContext _context;

        public DashboardManager(ApplicationDbContext context)
        {
            _context = context;
        }

        #region
        public async Task<string> GetDenumireClient(string id)
        {
            var client = await _context.Client.FindAsync(int.Parse(id));
            return client.Denumire;
        }

        public async Task<IList> GetProfitPierdere(string id, string an)
        {
            var profitPierdere = await _context.ProfitPierdere.Where(u => u.ClientId == int.Parse(id) && u.Year == an).ToListAsync();
            IList<float> pp = new List<float>();

            foreach (var pr in profitPierdere)
            {
                // daca avem pierdere adaugam pierderea
                if (pr.Profit_luna == null)
                {
                    pp.Add(pr.Pierdere_luna.Value);
                }
                // adaugam profitul
                else
                {
                    pp.Add(pr.Profit_luna.Value);
                }
            }

            return pp.ToArray();
        }

        public async Task<IList<List<float>>> GetSolduriCasa(string id, string an)
        {
            var solduriCasa = await _context.SolduriCasa
                .Where(u => u.data.Year.ToString() == an && u.ClientId.ToString() == id)
                .OrderBy(u => u.data.Month)
                .ToListAsync();

            IList<float> incasari = new List<float>();
            IList<float> plati = new List<float>();
            IList<float> solduriZi = new List<float>();

            foreach (var sold in solduriCasa)
            {
                incasari.Add(sold.incasari);
                plati.Add(sold.plati);
                solduriZi.Add(sold.sold_zi);
            }

            IList<List<float>> rezultat = new List<List<float>>() { 
                incasari.ToList(),
                plati.ToList(),
                solduriZi.ToList()
            };

            return rezultat;
        }


        #endregion
    }
}
