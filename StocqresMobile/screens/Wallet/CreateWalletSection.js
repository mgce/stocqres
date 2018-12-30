import React, { Component } from "react";
import { StyleSheet, View } from "react-native";
import { Text, Button, Form, Label, Input, Item } from "native-base";
import { fonts, colors, padding } from "../../styles/common";

export default CreateWalletSection = props => (
  <View>
    <View style={styles.textContainer}>
      <Text style={styles.header}>Create your wallet first</Text>
      <Text style={styles.paragraph}>
        Provide wallet amount and click "create"!
      </Text>
    </View>
    <Form>
      <Item floatingLabel>
        <Label>Wallet Amount</Label>
        <Input onChangeText={amount => props.onAmountChange(amount)} />
      </Item>
      <Button
        success
        style={styles.button}
        onPress={() => {
          props.createWallet(this.state.amount);
        }}
      >
        <Text>Create</Text>
      </Button>
    </Form>
  </View>
);

const styles = StyleSheet.create({
  header: {
    fontSize: fonts.lg,
    color: colors.darkPrimary
  },
  paragraph: {
    fontSize: fonts.md,
    color: colors.darkSecondary
  },
  textContainer: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center"
  },
  button: {
    width: "100%",
    alignSelf: "stretch",
    justifyContent: "center",
    paddingVertical: -padding.sm,
    marginTop: padding.md
  }
});
