package com.ramseyboy.vulcanscienceacademy

import groovy.text.GStringTemplateEngine

/**
 * Created by walkerhannan on 4/16/14.
 */
public class Parser {

    def parseFile(reqFileName, templateFileName) {

        def req = new File(reqFileName.getFile())
        def temp = new File(templateFileName.getFile())

        def methods = new ArrayList()

//        req.each {
//            methods.add(it)
//        }

        methods.add(req.text)

        def spockMethods = new ArrayList<String>()

        methods.eachWithIndex { obj, i ->
            def binding = [requirement:obj]
            def engine = new GStringTemplateEngine()
            def template = engine.createTemplate(temp).make(binding)
            spockMethods.add(template.toString())
        }

        new File("src/main/resources/results.groovy").withWriter { out ->
            spockMethods.each() { value ->
                out.writeLine(value)
            }
        }
    }
}