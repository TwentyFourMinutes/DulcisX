namespace DulcisX.Components.Events
{
    internal abstract class BaseEventX
    {
        protected uint CookieUID { get; set; }

        protected SolutionX Solution { get; }

        private protected BaseEventX(SolutionX solution)
            => Solution = solution;
    }
}
