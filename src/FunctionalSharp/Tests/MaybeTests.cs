using FunctionalSharp;
using Shouldly;
using System.Drawing;

namespace Tests;

public class MaybeTests
{
    [Test]
    public void Deve_HasValue_como_false()
    {
        string valorNulo = null;
        Maybe<string> maybe = valorNulo;

        maybe.HasValue.ShouldBeFalse();
    }

    [Test]
    public void Deve_HasValue_como_true()
    {
        string valor = "teste";
        Maybe<string> maybe = valor;

        maybe.HasValue.ShouldBeTrue();
    }

    [Test]
    public void Deve_alterar_valor_na_pipeline()
    {
        Maybe<string> maybe = "teste";
        var novo = maybe.Then(s => s.ToUpper()).GetValueOrElse("");

        novo.ShouldBe("TESTE");
    }

    [Test]
    public void Deve_receber_maybe_na_pipeline()
    {
        Maybe<int> intMaybe = 99;
        Maybe<string> maybe = "teste";
        var novo = maybe
                    .Then(s => intMaybe)
                    .GetValueOrElse(0);

        novo.ShouldBe(99);
    }

    [Test]
    public void Deve_fazer_pattern_match_do_resultado()
    {
        string valorNulo = null;
        Maybe<string> maybe1 = valorNulo;
        var result = maybe1.Match(some: s => s, none: () => "none");
        result.ShouldBe("none");

        string valor = "teste";
        Maybe<string> maybe2 = valor;
        var result2 = maybe2.Match(some: s => s, none: () => "none");
        result2.ShouldBe("teste");
    }
}
