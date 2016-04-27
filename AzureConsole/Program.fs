module DbTesting.Test

open System
open System.Linq
open System.Net
open System.Threading.Tasks
open Newtonsoft.Json
open Microsoft.Azure.Documents
open Microsoft.Azure.Documents.Client

let writeToConsoleAndPromptToContinue (msg:string) =
  printfn "%+A" msg
  printfn "Press any key to continue ..."
  let key = Console.ReadKey ()
  ()

let createDatabaseIfNotExists (client:DocumentClient) databaseName =
  try
    let readDb =
      client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName)) |> Async.AwaitTask
      

    writeToConsoleAndPromptToContinue ("Found: " + databaseName)
    readDb
  with
  | :? DocumentClientException ->
    let db = Database() 
    db.Id <- databaseName
    let createDb = client.CreateDatabaseAsync(db) |> Async.AwaitTask

    writeToConsoleAndPromptToContinue ("Created: " + databaseName)
    createDb
            
let getStartedDemo e (p:string) =
  let client = new DocumentClient(Uri(e), p)
  let resp = createDatabaseIfNotExists client "JobDb" |> Async.RunSynchronously
  resp, client



[<EntryPoint>]
let main args =
  let resp, client = getStartedDemo endpoint primaryKey
  0