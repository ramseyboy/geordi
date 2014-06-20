package com.ramseyboy.geordi

import com.ramseyboy.geordi.model.Feature
import com.ramseyboy.geordi.model.Specification
import org.yaml.snakeyaml.TypeDescription
import org.yaml.snakeyaml.Yaml
import org.yaml.snakeyaml.constructor.Constructor

/**
 * Created by walkerhannan on 4/17/14.
 */
class YamlParser {

    String fileName

    def parse() {
        Yaml yaml = new Yaml()

        Constructor constructor = new Constructor(Specification.class);
        TypeDescription specDescription = new TypeDescription(Specification.class);
        specDescription.putListPropertyType("features", Feature.class);
        constructor.addTypeDescription(specDescription)
        Specification spec = (Specification) yaml.load(this.getClass().getResource(fileName).text);

        return spec
    }
}
