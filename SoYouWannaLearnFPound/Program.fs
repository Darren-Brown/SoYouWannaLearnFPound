// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.


open System
open System.Text
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection

let mutable money = 10
let mutable bet = 1
let mutable playAgain = true
let mutable doubleDown = false

let maxRange = 10
let minRange = 3
let mutable currentRange = maxRange
let numberGenerator = new System.Random()

printfn "Welcome to higher/lower!"

while (playAgain && money > 0) do
    let parseYN x =
        match x with
            |"y" | "Y" -> true
            |"n" | "N" -> false

    let againValid x =
        match x with
          |"y" | "Y" | "n" | "N" -> true
          | a -> false 

    let guessParse x =
        match x with
        | "h" | "H" -> 1
        | "l" | "L" -> -1
        | "s" | "S" -> 0
        | a -> 2

    let getValidYN errorMessage =
        let mutable input = Console.ReadLine()
        while againValid input |> not do
            printfn errorMessage
            input <- Console.ReadLine()
        input |> parseYN

    let getValidGuess errorMessage =
        let mutable input = Console.ReadLine()
        while guessParse input = 2 do
            printfn errorMessage
            input <- Console.ReadLine()
        input |> guessParse

    doubleDown <- false
    playAgain <- false
    let (currentValue:int) =  numberGenerator.Next(1, currentRange)
    printfn "Current range is between 1 and %d" currentRange
    printfn "Current Value is %d. Will the next number be (h)igher, (l)ower, or the (s)ame?" currentValue

    let playerGuess = getValidGuess "Invalid selection. Please enter h ,  l, or s"
    let nextValue = numberGenerator.Next(1, currentRange)
    let evalValue = (nextValue - currentValue) * playerGuess
    printfn "The new number is %d" nextValue
    
    if evalValue > 0 || evalValue = (nextValue - currentValue) then
        printfn "You win! Your current winnings are %d." bet
        printfn "Credits: %d" money
        printfn "Use winnings as bet in next round (y/n)?"
        doubleDown <- getValidYN "Invalid selection. Would you like to use your winnings as the bet in the next round (y/n)?"
        if doubleDown then
            bet <- bet + bet
            playAgain <- true
            if currentRange > minRange then
                currentRange <- currentRange - 1
        else
            money <- money + bet
            bet <- 1
            currentRange <- maxRange
            printfn "Current credits: %d" money          
    else
        money <- money - 1
        printfn "You lose! Your current credit is %d." money
    if doubleDown |> not  then
        if money > 0 then
            printfn "Would you like to play again (y/n)?"
            playAgain <- getValidYN "Invalid selection. Would you like to play again (y/n)?"
        else
            playAgain <- false   
    printfn "Invalid selection. Please enter h ,  l, or s"

printfn "Thanks for playing."    
printfn "Press any key to continue..."
Console.ReadKey(true) |> ignore