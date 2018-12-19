import React from "react";
import { StyleSheet, View, TouchableOpacity } from "react-native";
import {
  Button,
  Text
} from "native-base";
import {padding, colors, fonts} from '../../styles/common'
import {goToStockDetails} from '../navigation';

export default StockListItem = props => {
    const price = Math.round(props.price * 100) / 100
    // const percentageRateItem = props.increased ? (
    //     <View style={listItemStyles.increasedPrice}>
    //     <Text>+ {props.percentageRate}%</Text>
    //   </View>) : (
    //     <View style={listItemStyles.descreasedPrice}>
    //     <Text>- {props.percentageRate}%</Text>
    //   </View>
    //   )
  return (
    <TouchableOpacity onPress={()=>goToStockDetails(props.componentId, props.name, props.code)}>
      <View style={listItemStyles.container}>
        <View style={listItemStyles.company}>
          <Text>{props.code}</Text>
          <Text style={listItemStyles.companyName}>{props.name}</Text>
        </View>
        <View style={listItemStyles.row}>
        <View>
            <Text>
                {price} zl
            </Text>
        </View>
          {/* {percentageRateItem} */}
        </View>
      </View>
    </TouchableOpacity>
  );
};

const listItemStyles = StyleSheet.create({
  container: {
    alignItems: "center",
    flex: 1,
    flexDirection: "row",
    justifyContent: "space-between",
    padding: padding.sm
  },
  company: {
    flexDirection: "column"
  },
  companyName: {
    fontSize: fonts.sm,
    color: colors.darkSecondary
  },
  row: {
    flexDirection: "row",
    alignItems: "center"
  },
  increasedPrice: {
    backgroundColor: 'green',
    color:colors.lightPrimary,
    fontSize: fonts.md,
    fontWeight: "bold",
    paddingLeft: padding.sm
  },
  descreasedPrice: {
    backgroundColor: 'red',
    color:colors.lightPrimary,
    fontSize: fonts.md,
    fontWeight: "bold",
    paddingLeft: padding.sm
  }
});