namespace NerdStore.WebApp.MVC.Tests.Config
{
    public class PageObjectModel
    {
        protected readonly SeleniumHelper Helper;

        protected PageObjectModel(SeleniumHelper helper)
        {
            Helper = helper;
        }

        public string ObterUrl()
        {
            return Helper.ObterUrl();
        }

        public void NavegarParaUrl(string url)
        {
            Helper.IrParaUrl(url);
        }
    }
}
