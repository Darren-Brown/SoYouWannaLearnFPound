// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.


open System
open System.Text
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection

let mutable total = 42

let mutable money = 10
let mutable playAgain = true
let numberGenerator = new System.Random()

while (playAgain && money > 0) do
    let againParse x =
        match x with
            |"y" | "Y" -> true
            | "n" | "N" -> false

    let againValid x =
        match x with
          |"y" | "Y" | "n" | "N" -> true
          | a -> false 

    let (currentValue:int) =  numberGenerator.Next(1, 10)
    printfn "Current Value is %d. Will the next number be (h)igher, (l)ower, or the (s)ame?" currentValue
    let guessParse x =
        match x with
        | "h" -> 1
        | "l" -> -1
        | "s" -> 0
        | a -> 2

    let playerGuess = Console.ReadLine() |> guessParse
    let nextValue = numberGenerator.Next(1, 10)
    let evalValue = (nextValue - currentValue) * playerGuess
    printfn "The new number is %d" nextValue
    if playerGuess <> 2 then
        if evalValue > 0 || evalValue = (nextValue - currentValue) then
            money <- money + 1
            printfn "You win! Your current credit is %d." money
        
        else
            money <- money - 1
            printfn "You lose! Your current credit is %d." money
            
        if money > 0 then
            printfn "Would you like to play again (y/n)?"
            let mutable input = Console.ReadLine()
            while againValid input |> not do
                printfn "Invalid selection. Would you like to play again (y/n)?"
                input <- Console.ReadLine()
            playAgain <- againParse input
        else
            playAgain <- false   
    else
        printfn "Invalid selection. Please enter h ,  l, or s"

printfn "Thanks for playing."    
printfn "Press any key to continue..."
Console.ReadKey(true) |> ignore