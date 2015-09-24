using Fixie;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BeerCellier.Tests
{
    class CustomConvention : Convention
    {
        public CustomConvention()
        {
            Classes
            .NameEndsWith("Tests");

            Methods
                .Where(method => method.IsVoid());

            Parameters
                .Add<FromInputAttributes>();
        }
    }

    class FromInputAttributes : ParameterSource
    {
        public IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            return method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);
        }
    }
}
