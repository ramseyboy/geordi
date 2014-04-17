package com.ramseyboy.geordi

import groovy.text.GStringTemplateEngine
import org.yaml.snakeyaml.Yaml

/**
 * Created by walkerhannan on 4/16/14.
 */
public class Generator {

    def generateTests(String reqFileName) {
        Yaml yaml = new Yaml();
        Map<String, List<String>> reqMap = yaml.load(this.getClass().getResource(reqFileName).text)
        List<String> reqList = reqMap.get("Specifications")
        String feature = reqMap.get("Feature")

        String specs = supplySpecs(reqList)

        def spec = supplyClass(feature, specs).toString()

        new File("src/test/groovy/${feature}.groovy").withWriter { out ->
            out.writeLine(spec)
        }
    }

    def supplyClass(String specName, String specs) {
        def classTemplate = this.getClass().getResource('/spec.template')

        def binding = [className:specName, methods:specs]
        def engine = new GStringTemplateEngine()
        return engine.createTemplate(classTemplate).make(binding)
    }

    def supplySpecs(List<String> reqList) {
        def methodTemplate = this.getClass().getResource('/method.template')

        def spockMethods = new ArrayList<String>()

        reqList.eachWithIndex { obj, i ->
            def binding = [requirement:obj]
            def engine = new GStringTemplateEngine()
            def template = engine.createTemplate(methodTemplate).make(binding)
            spockMethods.add(template.toString())
        }

        def methodBody = ""
        spockMethods.each { str ->
            methodBody += str
        }

        return methodBody
    }
}