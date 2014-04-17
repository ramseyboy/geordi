package com.ramseyboy.vulcanscienceacademy

/**
 * Created by walkerhannan on 4/16/14.
 */
class Main {

    static main(args)  {
        def parser = new Parser()
        parser.parseFile(
                this.getClass().getResource('/bdd_parser_test.template'),
                this.getClass().getResource('/test.template')
        )
    }
}
