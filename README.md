# MoneyArchive

Money Archive is a small Windows Forms app designed to allow viewing of an archive of an MS Money 99 database.

## History

I wrote it because I used MS Money 97 and then 99 from 1997 until 2021, when I decided to switch to using an online accounting package that could manage more functions, such as automatic connections to bank and credit card accounts, invoicing and VAT return submission. However, the main reason I decided to switch was that MS Money 99 was no longer running smoothly on my Windows 10 PC, even within an XP VM (it seemed to have problems with the display driver, which made it flicker and run very slowly). I didn't want to lose access to that almost 25 years of account data, so I needed some other way of getting at it.

It seems likely that the Money database is actually an encrypted Access mdb, but I've never been able to find even a claim that anyone has managed to crack the encryption. Therefore, the only way to get bulk data out of it is to use its [QIF](https://en.wikipedia.org/wiki/Quicken_Interchange_Format) export function. Having exported all the accounts to qif files, I then tried to find a free accounting package that I could import it into but after trying several that were unable to manage it despite claims that they could I decided to write my own. That's not actually so difficult as it only needed to be read-only.

Initially, I assumed that the qif files would need to be loaded into a database, and wrote a SQL Server Entity Framework Code First database and loader for them, but it turned out to load so fast that it is easier just to load the qif files directly into memory every time the app is run. The only slow part seems to be loading the grid with 10s of thousands of records, which wouldn't be faster with a true database anyway.

## Installation

At present, there is no install program. If anyone asks for one, I'll write it.

To install it at the moment, it will need to be built from the source. It was written using VS 2022 Preview. It can be placed under `Program Files`, and it will place its config data (currently just the last folder of qif files opened) in `%appdata%\Babbacombe Computers Ltd\MoneyArchive`. I'm assuming anyone who can do that can work out how to install it themselves.

## Preparing the QIF files

The QIF files need to be exported from MS Money. As far as I can see, this has to be done one account at a time, which can be tedious but shouldn't take more than a few minutes. Use the File/Export function and select "Loose QIF" for each file. It is important to ensure that the name of each file matches the name of the account in Money exactly (including case) because this is used to determine the "sides" of a transfer.

Once exported, put the qif files into a folder that will be accessible to MoneyArchive.

## Running the app

The first time the app is run it will ask for the location of the folder containing the qif files. It will automatically open this next time it is run. If you want to change it, or have more than one set of accounts, you can use the File/Open menu item to open a different folder.

If it all loads OK it will show a list of accounts on the left hand side, and a grid of transactions from the first account in the right hand side. Rather than always being by account, the combo box above the list allows selection by Payee or Category, as well as an option to list all the transactions from all accounts in one grid.

There a search box above the grid. Typing into that restricts the grid to transactions containing the text entered (it's just a simple case-insensitive search).

The transactions are always listed in date order. They may not be in exactly the correct order within a date. When loaded (ignoring search) a running total is added to each transaction corresponding to the currently selected display.

If a transaction is shown as "Split" in the final column, it contains Split data, breaking down the transaction. In this case, hovering the mouse over the transaction will show the split data (it's not a good display, but it was easy to implement - changing to a fixed font or displaying another grid seemed like too much work).

If a transaction is a transfer then double clicking on it should go to the other side of the transfer. This only works when the app is in Account display mode, and doesn't work if the other side of the transaction is a split (I had so few that I couldn't be bothered with implementing that). Opening Balance transactions are transfers to the same account (!) and double clicking won't do anything with those either.

## Other Restrictions

The app doesn't handle Investment accounts (because I only had one with only one opening balance transaction in it). It also only handles the QIF files from MS Money, not the extensions from Quicken and other packages.

Just to be clear, this app was not written to my normal programming standards. It is working with my data, which is good enough for me. If I were releasing it as a product it would have a lot more defensive code, exception trapping, and logging.