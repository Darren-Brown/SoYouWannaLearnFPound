// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.


open System
open System.Text
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection



let startingCredit = 10
let minBet = 1

let maxRange = 10
let minRange = 3
//let mutable currentRange = maxRange
let numberGenerator = new System.Random()

printfn "Welcome to higher/lower!"

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


let rec getValidYN errorMessage =
    let input = Console.ReadLine()
    if againValid input |> not then
        printfn errorMessage
        getValidYN errorMessage
    else
        input |> parseYN

let rec getValidGuess errorMessage =
    let mutable input = Console.ReadLine()
    if guessParse input = 2 then
        printfn errorMessage
        getValidGuess errorMessage
    else
        input |> guessParse

let getPlayAgain message =
    printfn message
    getValidYN "Invalid selection. Would you like to play again (y/n)?"

let adjustRange currentRange =
    if currentRange <= minRange then
        currentRange
    else
        currentRange - 1

let rec gameLoop currentRange money (bet:int) =
    let (currentValue:int) =  numberGenerator.Next(1, currentRange)
    printfn "Current range is between 1 and %d" currentRange
    printfn "Current Value is %d. Will the next number be (h)igher, (l)ower, or the (s)ame?" currentValue

    let playerGuess = getValidGuess "Invalid selection. Please enter h ,  l, or s"

    let nextValue = numberGenerator.Next(1, currentRange)
    let evalValue = (nextValue - currentValue) * playerGuess
    printfn "The new number is %d" nextValue
    let winner = evalValue > 0 || evalValue = (nextValue - currentValue)

    if winner then
        //don't care
        printfn "You win! Your current winnings are %d." bet
        printfn "Credits: %d" money
        printfn "Use winnings as bet in next round (y/n)?"
        let doubleDown = getValidYN "Invalid selection. Would you like to use your winnings as the bet in the next round (y/n)?"
        if doubleDown then
            let newBet = bet + bet
            let newMoney = money
            let newRange = adjustRange currentRange
            gameLoop newRange newMoney newBet
        else
            let newMoney = money + bet
            let newBet = minBet
            let newRange = maxRange
            if getPlayAgain "Would you like to play again (y/n)?" then
                gameLoop newRange newMoney newBet
            else
                newMoney
    else
        //also don't care
        let newMoney = money - 1
        let newRange = maxRange
        let newBet = minBet
        printfn "You lose! Your current credit is %d." newMoney
        if getPlayAgain "Would you like to play again (y/n)?" && newMoney > 0 then
            gameLoop newRange newMoney newBet
        else
            newMoney

let test = gameLoop maxRange startingCredit minBet
printfn "Thanks for playing! You walked away with %d credits" test 
printfn "Press any key to continue..."
Console.ReadKey(true) |> ignore