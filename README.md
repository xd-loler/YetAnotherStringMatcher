# YetAnotherStringMatcher
 
YetAnotherStringMatcher library tries to make string matching easier, more readable while being somewhat expressive.

It tries to replace Regex to some extent, mostly when it comes to relatively basic cases.

For now it is in very early version and apart from the errors, then it may lack of "obvious things" and be kinda inconsistent in its APIs

Source Code: https://github.com/xd-loler/YetAnotherStringMatcher/tree/master/src/Core

Tests: https://github.com/xd-loler/YetAnotherStringMatcher/blob/master/src/Tests/BasicTests.cs

## How to install

* .NET CLI: `dotnet add package YetAnotherStringMatcher --version 1.0.3`

* Package Manager: `Install-Package YetAnotherStringMatcher -Version 1.0.3`

* Package Reference: `<PackageReference Include="YetAnotherStringMatcher" Version="1.0.3" />`

* Nuget.org URL: https://www.nuget.org/packages/YetAnotherStringMatcher/

## Some random examples, more in specific API description below

```csharp
// Polish Postal Code
var input = "12-345";

var result = new Matcher(input)
                 .MatchDigitsOfLength(2)
                 .Then("-")
                 .ThenDigitsOfLength(3)
                 .Check();

Assert.True(result.Success);
```

```csharp
var matcher = new Matcher("[2021-09-05] ERROR: Message1! Exception!")
                  .Match("[2021-09-05]")
                  .ThenAnything()
                  .Then("Exception!")
                  .Check();
				  
Assert.True(matcher.Success);
```

```csharp
var input = new List<string>
{
	"[2021-09-05] ERROR: Message1",
	"[2021-09-05] WARNING: Message1",
	"[2021-09-05] INFO: Message1",
	"[2021-09-07] WARNING: Message1",
};

var pattern = new Matcher()
		  .Match("[2021-09-05] ")
		  .ThenAnyOf("WARNING:", "ERROR:");

Assert.True(pattern.Check(input[0]).Success);
Assert.True(pattern.Check(input[1]).Success);

Assert.False(pattern.Check(input[2]).Success);
Assert.False(pattern.Check(input[3]).Success);
```

```csharp
var matcher = new Matcher("Apple Watermelon")
                  .MatchAnyOf("Apple", "Banana")
                  .Then(" ")
                  .ThenAnyOf("Giraffe", "melon", "Watermelon")
                  .Check();

Assert.True(matcher.Success);
```

```csharp
var matcher = new Matcher("TEST")
                  .Match("test").IgnoreCase()
                  .Check();

Assert.True(matcher.Success);
```

```csharp
// Sample Phone Number / Reusable Pattern
var input = new List<string> { "+123 345 67 89", "+1424 345 67 89" };

var pattern = new Matcher()
                  .Match("+")
                  .ThenDigitsOfLength(3)
                  .Then(" ")
                  .ThenDigitsOfLength(3)
                  .Then(" ")
                  .ThenDigitsOfLength(2)
                  .Then(" ")
                  .ThenDigitsOfLength(2);

Assert.True(pattern.Check(input[0]).Success);
Assert.False(pattern.Check(input[1]).Success);
```

```csharp
var result = new Matcher("Apple Pineapple")
                .Match("Apple ")
                .Then("Coconut ").IsOptional()
                .Then("Pineapple")
                .Check();

Assert.True(result.Success);
```

```csharp
var str = @"RROR: duplicate key value violates unique...
            Detail: Key (some_column)=(b01a0e23-da71-3a08-9893-11b8b2dfb069) already exists.";

var check = new Matcher(str)
                .MatchAnything()
                .Then("duplicate key value violates unique")
                .ThenAnything()
                .Then("Detail: Key ")
                .ThenExtract().ExtractAs("output")
                .Then(" already exists.")
                .Check();

Assert.True(check.Success);
Assert.Equal("(some_column)=(b01a0e23-da71-3a08-9893-11b8b2dfb069)", check.ExtractedData["output"]);
```

```csharp
var result = new Matcher("Apple Water_Banana Watermelon")
		 .Match("Apple")
		 .ThenAnything()
		 .Then("Water")
		 .Then("melon")
		 .Check();
				 
Assert.True(result.Success);
```

# Avaliable APIs:

**Match / Then** - tries to match exact string

```csharp
var matcher = new Matcher("ERROR: Exception 1...")
                  .Match("ERROR: Exception ")
                  .ThenAnyOf("1", "2", "3")
                  .Check();

Assert.True(matcher.Success);
```

___


**MatchAnyOf / ThenAnyOf** - tries to match longest possible element of list handed as parameter.

```csharp
var matcher = new Matcher("Apple Watermelon")
                  .MatchAnyOf("Apple", "Banana")
                  .Then(" ")
                  .ThenAnyOf("Giraffe", "melon", "Watermelon")
                  .Check();

Assert.True(matcher.Success);
```
___

**MatchAnything / ThenAnything** - matches anything non empty

```csharp
var matcher = new Matcher("[2021-09-05] ERROR: Message1! Exception!")
                  .Match("[2021-09-05]")
                  .ThenAnything()
                  .Then("Exception!")
                  .Check();
				  
Assert.True(matcher.Success);
```

**This one will FAIL:**
```csharp
var matcher = new Matcher("12")
                  .Match("1")
                  .ThenAnything()
                  .Then("2")
                  .Check();
				  
Assert.False(matcher.Success);
```

___

**ThenExtract().ExtractAs(...)** - next symbols will be extracted and returned as element of dictionary with under key provided by ExtractAs
```csharp
var str = @"RROR: duplicate key value violates unique...
            Detail: Key (some_column)=(b01a0e23-da71-3a08-9893-11b8b2dfb069) already exists.";

var check = new Matcher(str)
                .MatchAnything()
                .Then("duplicate key value violates unique")
                .ThenAnything()
                .Then("Detail: Key ")
                .ThenExtract().ExtractAs("output")
                .Then(" already exists.")
                .Check();

Assert.True(check.Success);
Assert.Equal("(some_column)=(b01a0e23-da71-3a08-9893-11b8b2dfb069)", check.ExtractedData["output"]);
```

___

**MatchAnythingOfLength / ThenAnythingOfLength** - matches anything that has expected length

```csharp
var matcher = new Matcher("01-000 London")
                  .Match("01")
                  .ThenAnythingOfLength(5)
                  .Then("London")
                  .Check();
				  
Assert.True(matcher.Success);
```

___

**MatchDigitsOfLength / ThenDigitsOfLength** - matches digits of expected length

```csharp
var matcher = new Matcher("01-000 London")
                  .MatchDigitsOfLength(2)
                  .Then("-")
                  .MatchDigitsOfLength(3)
                  .Check();
				  
Assert.True(matcher.Success);
```

**This one will FAIL:**

```csharp
var matcher = new Matcher("aa-000 London")
                  .MatchDigitsOfLength(2)
                  .Then("-")
                  .MatchDigitsOfLength(3)
                  .Check();

Assert.False(matcher.Success);
```

___

**MatchDigitsWithLengthBetween / ThenDigitsWithLengthBetween** - matches digits with length between [A...B]

```csharp
var input = new List<string> { "12-000 London", "2-00 Zurich" };

var pattern = new Matcher()
                  .MatchDigitsWithLengthBetween(1, 2)
                  .Then("-")
                  .MatchDigitsWithLengthBetween(1, 3);

Assert.True(pattern.Check(input[0]).Success);
Assert.True(pattern.Check(input[1]).Success);
```

**This one will FAIL:**

```csharp
var input = new List<string> { "121-000 London", "44-0011 Zurich" };

var pattern = new Matcher()
                  .MatchDigitsWithLengthBetween(1, 2)
                  .Then("-") // because it isn't on 3rd position
                  .MatchDigitsWithLengthBetween(1, 3);

Assert.False(pattern.Check(input[0]).Success);
```

___

**MatchSymbolsOfLength / ThenSymbolsOfLength** - matches symbols that are provided

```csharp
var input = new List<string> { "+-123" };

var pattern = new Matcher()
                  .MatchSymbolsOfLength(new[] { '+','-'}, 2)
                  .MatchDigitsWithLengthBetween(1, 3)
                  .NoMore();

Assert.True(pattern.Check(input[0]).Success);
```

**This one will FAIL:**

```csharp
var input = new List<string> { "+-123" };

var pattern = new Matcher()
                  .MatchSymbolsOfLength(new[] { '!','@'}, 2)
                  .MatchDigitsWithLengthBetween(1, 3)
                  .NoMore();

Assert.False(pattern.Check(input[0]).Success);
```

___

**MatchCustomOfLength / ThenCustomOfLength** - matches symbols that satisfy given predicate

```csharp
Func<char, CheckOptions, bool> func = (char c, CheckOptions _) => char.IsUpper(c);

var matcher = new Matcher("ABC")
                  .MatchCustomOfLength(func, 3)
                  .Check();

Assert.True(matcher.Success);
```

**This one will FAIL:**

```csharp
Func<char, CheckOptions, bool> func = (char c, CheckOptions _) => char.IsUpper(c);

var matcher = new Matcher("abc")
                  .MatchCustomOfLength(func, 3)
                  .Check();

Assert.False(matcher.Success);
```

___

**NoMore** - string must end here

```csharp
var matcher = new Matcher("TEST123")
                  .Match("TEST")
                  .NoMore()
                  .Check();

Assert.False(matcher.Success);
```

___

**IsOptional** - matching will not fail if this particular element is not present

```csharp
var result = new Matcher("Apple Pineapple")
                .Match("Apple ")
                .Then("Coconut ").IsOptional()
                .Then("Pineapple")
                .Check();

Assert.True(result.Success);
```

___

**IgnoreCase** - Ignores Case

```csharp
var matcher = new Matcher("TEST")
                  .Match("test").IgnoreCase()
                  .Check();

Assert.True(matcher.Success);
```

___

**IgnoreCaseForAllExisting** - Ignores Case for all previous requirements

```csharp
var matcher = new Matcher("TEST_ABC")
                  .Match("test_")
                  .Match("abc")
                  .IgnoreCaseForAllExisting()
                  .Check();

Assert.True(matcher.Success);
```

___

**ExtractAs** - read description of **ThenExtract()**

___

**ThenCustom** - You can provide your own implementation of IRequirement interface.

___

# FAQ

* Should I use this on prod? Probably not (yet)

* Why does it use old C#? I wanted it to work even on old .NET


# To Do List

* Improve error handling / add some additional information why given step failed

* Support more complex operations like ThenAnyOf(IRequirement, IRequirement, IRequirement...)

* Perform Benchmarks between this and Regex (System.Text.RegularExpressions)

* Complete Regex Generation https://github.com/xd-loler/YetAnotherStringMatcher/tree/master/src/RegexCodeGeneration

# Experimental Features

It is possible to generate pure Regex from Matcher itself, but it will probably not support all
kind of requirements because `SomethingOfLenghtRequirement` receives `Func<char, CheckOptions, bool>`
which would be pretty diffcult to convert into Regex.

```csharp
var text = "APPLE1";

var matcher = new Matcher()
                    .MatchAnyOf("Apple", "Watermelon").IgnoreCase();

var generator = new RegexGenerator(matcher.GetRequirements.ToList());

var result = generator.Emit();

Assert.True(result.Success);
Assert.False(string.IsNullOrWhiteSpace(result.Code));

// ("Apple", "Watermelon") are ordered by length.
Assert.Equal("(?i)(Watermelon|Apple)", result.Code);

var regex = new Regex(result.Code);

Assert.True(matcher.Check(text).Success);
Assert.Matches(regex, text);
```


```csharp
var matcher = new Matcher()
                .Match("[2021-09-05] ")
                .ThenAnyOf("WARNING:", "ERROR:");

var generator = new RegexGenerator(matcher.GetRequirements.ToList());
var result = generator.Emit();
Console.WriteLine(result.Code); // ([2021-09-05] )(WARNING:|ERROR:)
```

```csharp
var matcher = new Matcher()
                .MatchAnyOf("Apple", "Banana")
                .Then(" ")
                .ThenAnyOf("Giraffe", "melon", "Watermelon");

var generator = new RegexGenerator(matcher.GetRequirements.ToList());
var result = generator.Emit();
Console.WriteLine(result.Code); // (Banana|Apple)( )(Watermelon|Giraffe|melon)
```


Source code: https://github.com/xd-loler/YetAnotherStringMatcher/tree/master/src/RegexCodeGeneration

Tests: https://github.com/xd-loler/YetAnotherStringMatcher/blob/master/src/Tests/RegexCodeGenTests.cs

# Some Naive Benchmarks

Source Code: https://github.com/xd-loler/YetAnotherStringMatcher/tree/master/src/Benchmarking

## Results 

``` ini
.NET SDK=6.0.100-preview.5.21302.13
  [Host]     : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT
  DefaultJob : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT
```

**Compiled:**

|      Method |            text |      Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------ |---------------- |----------:|---------:|---------:|-------:|----------:|
|  Case1_YASM |           Apple |  73.05 ns | 1.140 ns | 1.067 ns | 0.0381 |     160 B |
| Case1_Regex |           Apple |  53.99 ns | 0.603 ns | 0.564 ns |      - |         - |
|  Case1_YASM | AppleWatermelon |  75.12 ns | 0.572 ns | 0.478 ns | 0.0381 |     160 B |
| Case1_Regex | AppleWatermelon |  53.64 ns | 0.395 ns | 0.369 ns |      - |         - |
|  Case1_YASM |      Watermelon | 311.46 ns | 1.126 ns | 0.879 ns | 0.0877 |     368 B |
| Case1_Regex |      Watermelon |  46.79 ns | 0.386 ns | 0.342 ns |      - |         - |


|      Method |                 text |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------ |--------------------- |------------:|----------:|----------:|-------:|----------:|
|  Case2_YASM | 12312(...)Apple [28] | 1,029.61 ns | 20.388 ns | 21.815 ns | 0.2575 |   1,080 B |
| Case2_Regex | 12312(...)Apple [28] |    46.84 ns |  0.394 ns |  0.308 ns |      - |         - |
|  Case2_YASM | qqqqq(...)melon [30] |   594.30 ns |  7.961 ns |  7.447 ns | 0.1812 |     760 B |
| Case2_Regex | qqqqq(...)melon [30] |    47.57 ns |  0.567 ns |  0.502 ns |      - |         - |
|  Case2_YASM | xcvxc(...)melon [44] | 1,728.00 ns | 16.347 ns | 15.291 ns | 0.5093 |   2,136 B |
| Case2_Regex | xcvxc(...)melon [44] |    47.03 ns |  0.280 ns |  0.262 ns |      - |         - |

|      Method |                 text |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------ |--------------------- |------------:|----------:|----------:|-------:|----------:|
|  Case3_YASM | 12312(...)pple3 [29] | 1,065.93 ns | 15.903 ns | 14.875 ns | 0.3033 |   1,272 B |
| Case3_Regex | 12312(...)pple3 [29] |    63.71 ns |  0.648 ns |  0.606 ns |      - |         - |
|  Case3_YASM | qqqqq(...)melon [32] |   720.06 ns | 10.396 ns |  9.724 ns | 0.2270 |     952 B |
| Case3_Regex | qqqqq(...)melon [32] |    59.86 ns |  0.697 ns |  0.652 ns |      - |         - |
|  Case3_YASM | xcvxc(...)melon [44] | 1,852.92 ns | 21.209 ns | 19.839 ns | 0.5093 |   2,136 B |
| Case3_Regex | xcvxc(...)melon [44] |    39.03 ns |  0.494 ns |  0.438 ns |      - |         - |

|      Method |                 text |      Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------ |--------------------- |----------:|---------:|---------:|-------:|----------:|
|  Case4_YASM | Lette(...)ondon [22] | 348.59 ns | 3.678 ns | 3.260 ns | 0.1106 |     464 B |
| Case4_Regex | Lette(...)ondon [22] |  65.44 ns | 0.722 ns | 0.675 ns |      - |         - |
|  Case4_YASM | Parce(...)ondon [22] | 547.35 ns | 8.901 ns | 7.891 ns | 0.1431 |     600 B |
| Case4_Regex | Parce(...)ondon [22] | 231.41 ns | 1.582 ns | 1.479 ns |      - |         - |
|  Case4_YASM |         ParcelLondon | 264.08 ns | 3.405 ns | 2.658 ns | 0.1049 |     440 B |
| Case4_Regex |         ParcelLondon |  85.63 ns | 0.639 ns | 0.566 ns |      - |         - |


**Not Compiled:**

|      Method |            text |       Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------ |---------------- |-----------:|---------:|---------:|-------:|----------:|
|  Case1_YASM |           Apple |   761.3 ns | 10.47 ns |  9.28 ns | 0.2537 |      1 KB |
| Case1_Regex |           Apple | 1,928.2 ns | 32.31 ns | 35.91 ns | 0.6485 |      3 KB |
|  Case1_YASM | AppleWatermelon |   744.4 ns | 14.63 ns | 21.45 ns | 0.2537 |      1 KB |
| Case1_Regex | AppleWatermelon | 1,868.8 ns |  8.65 ns |  7.67 ns | 0.6485 |      3 KB |
|  Case1_YASM |      Watermelon | 1,049.0 ns |  9.55 ns |  8.94 ns | 0.3033 |      1 KB |
| Case1_Regex |      Watermelon | 1,698.6 ns | 12.28 ns | 11.49 ns | 0.4978 |      2 KB |

|      Method |                 text |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------ |--------------------- |---------:|----------:|----------:|-------:|----------:|
|  Case2_YASM | 12312(...)Apple [28] | 1.915 μs | 0.0222 μs | 0.0208 μs | 0.5035 |      2 KB |
| Case2_Regex | 12312(...)Apple [28] | 1.483 μs | 0.0122 μs | 0.0108 μs | 0.6046 |      2 KB |
|  Case2_YASM | qqqqq(...)melon [30] | 1.494 μs | 0.0141 μs | 0.0132 μs | 0.4292 |      2 KB |
| Case2_Regex | qqqqq(...)melon [30] | 1.533 μs | 0.0100 μs | 0.0094 μs | 0.6046 |      2 KB |
|  Case2_YASM | xcvxc(...)melon [44] | 2.629 μs | 0.0361 μs | 0.0320 μs | 0.7591 |      3 KB |
| Case2_Regex | xcvxc(...)melon [44] | 1.307 μs | 0.0125 μs | 0.0111 μs | 0.4539 |      2 KB |


|      Method |                 text |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------ |--------------------- |---------:|----------:|----------:|-------:|----------:|
|  Case3_YASM | 12312(...)pple3 [29] | 2.254 μs | 0.0211 μs | 0.0198 μs | 0.5722 |      2 KB |
| Case3_Regex | 12312(...)pple3 [29] | 2.032 μs | 0.0126 μs | 0.0106 μs | 0.6676 |      3 KB |
|  Case3_YASM | qqqqq(...)melon [32] | 1.831 μs | 0.0324 μs | 0.0287 μs | 0.4978 |      2 KB |
| Case3_Regex | qqqqq(...)melon [32] | 2.013 μs | 0.0401 μs | 0.0478 μs | 0.6676 |      3 KB |
|  Case3_YASM | xcvxc(...)melon [44] | 2.770 μs | 0.0169 μs | 0.0132 μs | 0.7820 |      3 KB |
| Case3_Regex | xcvxc(...)melon [44] | 1.813 μs | 0.0106 μs | 0.0099 μs | 0.5035 |      2 KB |

|      Method |                 text |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------ |--------------------- |---------:|----------:|----------:|-------:|----------:|
|  Case4_YASM | Lette(...)ondon [22] | 2.098 μs | 0.0122 μs | 0.0102 μs | 0.5493 |      2 KB |
| Case4_Regex | Lette(...)ondon [22] | 5.163 μs | 0.0573 μs | 0.0536 μs | 1.3046 |      5 KB |
|  Case4_YASM | Parce(...)ondon [22] | 2.320 μs | 0.0149 μs | 0.0124 μs | 0.5836 |      2 KB |
| Case4_Regex | Parce(...)ondon [22] | 6.136 μs | 0.0673 μs | 0.0629 μs | 1.3199 |      5 KB |
|  Case4_YASM |         ParcelLondon | 2.013 μs | 0.0193 μs | 0.0181 μs | 0.5455 |      2 KB |
| Case4_Regex |         ParcelLondon | 5.068 μs | 0.0535 μs | 0.0474 μs | 1.3199 |      5 KB |

## Benchmark FAQ

* How to run Benchmark on my own computer?

Run this command inside `\Benchmarking` folder

``` ini
dotnet run -c Release
```

