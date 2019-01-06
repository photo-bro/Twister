# **Twister**

*Twister is a basic programming language heavily modeled after C. In this repository is a basic Twister to x86_64 assembly compiler. All design and code contained currently has been solely created by Josh Harmon. This project is nothing more than a fun and educational exercise.*

## **Build & Test Status:**

*Twister is built on  .NetCore 2.1 and targets .Net Standard 2.0 compliance.* 

[![Build Status](https://travis-ci.org/photo-bro/Twister.svg?branch=master)](https://travis-ci.org/photo-bro/Twister)

## **Current Status:**

*(1/5/19)*
More work on the parse. A bit of feeling my way as I write but feel like I'm narrowing myself on the right path. Finished implementing all of TwisterPrimitive. Fine tuning the grammar too.

*(1/4/19)*
Initial implementation of parser has began. TwisterPrimitive struct is being built up to represent all primitive types of the language. I am taking advantage of C#'s operator overload to hide some of the complexity and boilerplate of writing all the arithmetic operational code.

## **Intent:**

Design and implement simple but full featured language with standard primitive types, arithmetic abilities, and control flow structures. 

## **Goals:**

 Support following primitive types:
   - Integers
   - Floating points
   - Characters
   - Strings

- Support following complex types:
   - Arrays
   - Structs

 - Support functions
 -  Support library includes
 - Include limited standard library 


## **Stretch Goals:**

 - Heap allocation / freeing

## **(Super) Stretch Goals**

 - Bootstrap compiler
 - Reference counting / garbage collection

