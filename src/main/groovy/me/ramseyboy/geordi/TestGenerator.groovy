package me.ramseyboy.geordi

import com.squareup.javapoet.JavaFile
import com.squareup.javapoet.MethodSpec
import com.squareup.javapoet.TypeSpec
import groovy.transform.CompileStatic
import me.ramseyboy.geordi.model.Feature
import org.junit.Assert
import org.junit.Test

import javax.lang.model.element.Modifier

@CompileStatic
class TestGenerator {

    def generateTests(String reqFileName) {
        def parser = new SpecificationParser()
        def spec = parser.parse(reqFileName)

        def features = spec.features;

        List<MethodSpec> methods = features.collect { feature ->
            return MethodSpec.methodBuilder(feature.feature.toLowerCase().replaceAll(" ", "_").replaceAll(",", ""))
                    .addModifiers(Modifier.PUBLIC)
                    .returns(void.class)
                    .addAnnotation(Test.class)
                    .addStatement("\$T.assertTrue(\$L)", Assert.class, true)
                    .addException(Exception.class)
                    .build();
        }

        TypeSpec helloWorld = TypeSpec.classBuilder("${spec.specification}Test")
                .addModifiers(Modifier.PUBLIC, Modifier.FINAL)
                .addMethods(methods)
                .build();

        def javaFile = JavaFile.builder("me.ramseyboy.geordi", helloWorld)
                .build();

        javaFile.writeTo(new File("src/test/java/"))
    }
}