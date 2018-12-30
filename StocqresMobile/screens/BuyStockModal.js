import React, { Component } from "react";
import { StyleSheet, Dimensions } from "react-native";
import Modal from "react-native-modal";
import { Item, Label, Input, Button, Text, View } from "native-base";
import { padding, colors, fonts } from "../styles/common";
import _ from "lodash";

const height = Dimensions.get("window").height;
const width = Dimensions.get("window").width;

export default (BuyStockModal = props => (
  <Modal
    isVisible={props.isVisible}
    onBackdropPress={() => props.toggleModal(false)}
  >
    <View style={styles.modal}>
      <View style={styles.content}>
        <Text style={styles.header}>Buy Stocks</Text>
        <View style={styles.form}>
          <Text>How many unit of stocks would you like to buy?</Text>
          <Item floatingLabel>
            <Label>Quantity</Label>
            <Input
              onChangeText={quantity => props.onQuantityChange(quantity)}
            />
          </Item>
          <Button
            success
            style={styles.button}
            onPress={() => props.submitAction()}
          >
            <Text>Buy</Text>
          </Button>
        </View>
      </View>
    </View>
  </Modal>
));

const styles = StyleSheet.create({
  modal: {
    flex: 1,
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center"
  },
  header: {
    fontSize: fonts.lg,
    fontWeight: "bold",
    backgroundColor: colors.darkPrimary,
    color: colors.lightPrimary,
    paddingHorizontal: padding.md,
    paddingVertical: padding.sm,
    marginBottom: padding.sm
  },
  content: {
    backgroundColor: colors.lightPrimary,
    width: width * 0.75,
    height: height / 3
  },
  form: {
    paddingHorizontal: padding.md,
    justifyContent: "center",
    alignItems: "center"
  },
  button: {
    marginTop: padding.md,
    borderRadius: 0,
    width: "100%",
    justifyContent: "center"
  },
  buttonText: {
    fontSize: fonts.md,
    alignSelf: "center"
  }
});
